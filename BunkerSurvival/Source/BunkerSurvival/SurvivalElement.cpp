// Fill out your copyright notice in the Description page of Project Settings.


#include "SurvivalElement.h"
#include "Kismet/GameplayStatics.h"
#include "Kismet/KismetMathLibrary.h"
#include "BunkerSurvivalCharacter.h"
#include "InteractTriggerBase.h"
#include "BunkerSurvivalGameManager.h"

// Sets default values
ASurvivalElement::ASurvivalElement()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	BeginPlayTriggerCheckDone = false;

	CurrentLevel = 1.0f;
	DecrementRate = 0.1f;
	CriticalLevel = 0.5f;
	HealthImpactRate = 0.0f;
}

// Called when the game starts or when spawned
void ASurvivalElement::BeginPlay()
{
	Super::BeginPlay();

	playerCharacter = Cast<ABunkerSurvivalCharacter>(UGameplayStatics::GetPlayerCharacter(GetWorld(), 0));
	if(bunkerSurvivalGameManagerClass)
		bunkerSurvivalGameManager = Cast<ABunkerSurvivalGameManager>(UGameplayStatics::GetActorOfClass(GetWorld(), bunkerSurvivalGameManagerClass));
	bunkerSurvivalGameManager->HealthDecrementRate = 0.0f;

	UGameplayStatics::GetAllActorsOfClass(GetWorld(), AInteractTriggerBase::StaticClass(), TriggersInWorld);

	//Randomising the triggers array, so they are enabled in random order.
	int destIndex = 0;
	for (int i=0; i<TriggersInWorld.Num()-1; i++)
	{
		destIndex = UKismetMathLibrary::RandomInteger64InRange(i+1, TriggersInWorld.Num()-2);
		TriggersInWorld.Swap(i, destIndex);
	}
}

// Called every frame
void ASurvivalElement::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	//If the first trigger is not enabled, enable it and reverse the flag.
	if (!BeginPlayTriggerCheckDone)
	{
		Cast<AInteractTriggerBase>(TriggersInWorld[0])->isInteractable = true;
		Cast<AInteractTriggerBase>(TriggersInWorld[0])->TriggerMesh->SetScalarParameterValueOnMaterials("FresnelMultiplier", 1.0);
		BeginPlayTriggerCheckDone = true;
	}

	//Decrement the element's current level if it is greater than 0.
	if(CurrentLevel>0)
		CurrentLevel -= DecrementRate * DeltaTime;

	//Start to decrement the health of player if current level drops below critical level.
	if (CurrentLevel < CriticalLevel)
	{
		TempHealthDecrementRate = (CriticalLevel - CurrentLevel) / 10 * DeltaTime;
		HealthImpactRate += TempHealthDecrementRate;
		bunkerSurvivalGameManager->HealthDecrementRate += TempHealthDecrementRate;
	}
	//Once current level comes above critical level, reset the health decrement rate of the player.
	else if(HealthImpactRate>0)
	{
		bunkerSurvivalGameManager->HealthDecrementRate -= HealthImpactRate;
		HealthImpactRate = 0.0f;
	}
}

void ASurvivalElement::EnableNextTrigger()
{
	//Removes the first trigger from the array and activate next trigger.
	TriggersInWorld.RemoveAt(0, 1, true);
	if (TriggersInWorld.Num() > 0)
	{
		Cast<AInteractTriggerBase>(TriggersInWorld[0])->isInteractable = true;
		Cast<AInteractTriggerBase>(TriggersInWorld[0])->TriggerMesh->SetScalarParameterValueOnMaterials("FresnelMultiplier", 1.0);
	}
}
