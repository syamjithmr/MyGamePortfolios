// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "BunkerSurvivalGameManager.generated.h"

UCLASS()
class BUNKERSURVIVAL_API ABunkerSurvivalGameManager : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ABunkerSurvivalGameManager();

	//In-Game HUD Component.
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Widgets")
		TSubclassOf<class UUserWidget> BunkerSurvivalHUDClass;
	UUserWidget* BunkerSurvivalHUD;
	//Game Over Widget Component.
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Widgets")
		TSubclassOf<class UUserWidget> BunkerSurvivalGameOverClass;
	UUserWidget* BunkerSurvivalGameOver;
	//Health of the character.
	UPROPERTY(VisibleAnywhere, BlueprintReadWrite)
		float Health;
	//Rate at which health will decrement.
	UPROPERTY(VisibleAnywhere, BlueprintReadWrite)
		float HealthDecrementRate;

	//Rpresent the Oxygen level in the game. Once the oxygen level goes below critical level, health will start to decrement.
	UPROPERTY(EditAnywhere)
		TSubclassOf<class ASurvivalElement> O2ElementActorClass;
	ASurvivalElement* O2ElementActor;

	//Time till the rescue team arrival.
	UPROPERTY(EditAnywhere)
		int TimeTillRescue;
	//Time to be displayed in In-Game HUD.
	UPROPERTY(BlueprintReadOnly)
		FString TimeTillRescueText;
	int minutes;
	int seconds;
	void ConvertSecondsToMinutes();
	//Timer to countdown to TimeTillRescue.
	FTimerHandle TimeTillRescueTimerHandle;
	void UpdateTimeTillRescue();

	class ABunkerSurvivalCharacter* playerCharacter;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};
