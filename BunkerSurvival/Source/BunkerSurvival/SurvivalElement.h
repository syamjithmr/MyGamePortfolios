// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "SurvivalElement.generated.h"

UCLASS()
class BUNKERSURVIVAL_API ASurvivalElement : public AActor
{
	GENERATED_BODY()
	
	//This class represents Survival element, which impact the health of player and decides which trigger to be enabled.

public:	
	// Sets default values for this actor's properties
	ASurvivalElement();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;
	//For distribution build, First Survival element glow effect was not getting activated when done inside BeginPlay().
	//So added that code inside Tick(). This flag is used to do this only once.
	bool BeginPlayTriggerCheckDone;

	//GameManager and character class references.
	class ABunkerSurvivalCharacter* playerCharacter;
	UPROPERTY(EditAnywhere)
		TSubclassOf<class ABunkerSurvivalGameManager> bunkerSurvivalGameManagerClass;
	class ABunkerSurvivalGameManager* bunkerSurvivalGameManager;

	//Array containing references to all trigger elements in world.
	TArray<AActor*> TriggersInWorld;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	//Enable the next trigger when current trigger is used.
	void EnableNextTrigger();

	//Current level of the element. This will decrement over time.
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
	float CurrentLevel;

	//Rate at which element level will decrease.
	UPROPERTY(EditAnywhere)
	float DecrementRate;

	//Once the level goes below critical level, health will start to decrement.
	UPROPERTY(EditAnywhere, BlueprintReadOnly)
		float CriticalLevel;

	//The rate at which the health is impacted.
	float HealthImpactRate;
	float TempHealthDecrementRate;

	//Currently active active trigger.
	class AInteractTriggerBase* CurrentTrigger;
};
