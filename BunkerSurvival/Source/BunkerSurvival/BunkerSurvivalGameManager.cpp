// Fill out your copyright notice in the Description page of Project Settings.

#include "BunkerSurvivalGameManager.h"
#include "Blueprint/UserWidget.h"
#include "Kismet/GameplayStatics.h"
#include "SurvivalElement.h"
#include "BunkerSurvivalCharacter.h"

// Sets default values
ABunkerSurvivalGameManager::ABunkerSurvivalGameManager()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	Health = 1.0f;
	HealthDecrementRate = 0.0f;

	TimeTillRescue = 30;
	minutes = 0;
	seconds = 0;
	TimeTillRescueText = FString::Printf(TEXT("00:00"));
}

// Called when the game starts or when spawned
void ABunkerSurvivalGameManager::BeginPlay()
{
	Super::BeginPlay();

	playerCharacter = Cast<ABunkerSurvivalCharacter>(UGameplayStatics::GetPlayerCharacter(GetWorld(), 0));

	//Initialize and display the In-Game HUD.
	if (BunkerSurvivalHUDClass)
	{
		BunkerSurvivalHUD = CreateWidget(GetWorld(), BunkerSurvivalHUDClass);

		if (BunkerSurvivalHUD)
		{
			BunkerSurvivalHUD->AddToViewport();
		}
	}

	if (O2ElementActorClass)
		O2ElementActor = Cast<ASurvivalElement>(UGameplayStatics::GetActorOfClass(GetWorld(), O2ElementActorClass));

	ConvertSecondsToMinutes();

	//Start the timer for counting down TimeTillRescue.
	GetWorldTimerManager().SetTimer(TimeTillRescueTimerHandle, this, &ABunkerSurvivalGameManager::UpdateTimeTillRescue, 1.0f, true);
}

// Called every frame
void ABunkerSurvivalGameManager::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	//Decrement health.
	if (Health > 0)
	{
		Health -= HealthDecrementRate * DeltaTime;
	}
	//If health is less than 0, end game.
	else
	{
		playerCharacter->isDead = true;
		GetWorldTimerManager().ClearTimer(TimeTillRescueTimerHandle);

		//Initialize and display Gameover Message.
		if (!BunkerSurvivalGameOver)
		{
			BunkerSurvivalHUD->RemoveFromViewport();

			if (BunkerSurvivalGameOverClass)
			{
				BunkerSurvivalGameOver = CreateWidget(GetWorld(), BunkerSurvivalGameOverClass);
			}

			if (BunkerSurvivalGameOver)
			{
				BunkerSurvivalGameOver->AddToViewport();
			}
		}
	}
}

void ABunkerSurvivalGameManager::ConvertSecondsToMinutes()
{
	//Convert TimeTillRescue to mm:ss format to be displayed in the HUD.
	minutes = TimeTillRescue / 60;
	seconds = TimeTillRescue % 60;
	TimeTillRescueText = FString::Printf(TEXT("%02d:%02d"), minutes, seconds);
}

void ABunkerSurvivalGameManager::UpdateTimeTillRescue()
{
	//Countdown
	TimeTillRescue--;
	ConvertSecondsToMinutes();

	//Check if the countdown have become 0. If it is, the game is won.
	if (TimeTillRescue < 1)
	{
		GetWorldTimerManager().ClearTimer(TimeTillRescueTimerHandle);
		playerCharacter->isWon = true;
		//Display Gamewon message.
		UGameplayStatics::OpenLevel(GetWorld(), "GameWonLevel");
	}
}
