// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/TriggerBox.h"
#include "InteractTriggerBase.generated.h"

/**
 * Base class for all trigger objects
 */
UCLASS()
class BUNKERSURVIVAL_API AInteractTriggerBase : public ATriggerBox
{
	GENERATED_BODY()
	
protected:

	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	//Called when player interacts with the trigger.
	void Interact();

public:

	// constructor sets default values for this actor's properties
	AInteractTriggerBase();

	// declare overlap begin function
	UFUNCTION()
		void OnOverlapBegin(class AActor* OverlappedActor, class AActor* OtherActor);

	// declare overlap end function
	UFUNCTION()
		void OnOverlapEnd(class AActor* OverlappedActor, class AActor* OtherActor);

	//Text displayed above the trigger object, when the player enter the trigger.
	UPROPERTY(EditDefaultsOnly)
	class UTextRenderComponent* TriggerBillboardTextRender;

	//The object displayed in the world for the trigger.
	UPROPERTY(EditDefaultsOnly)
		UStaticMeshComponent* TriggerMesh;

	//The SurvivalElement which manages this trigger.
	UPROPERTY(EditAnywhere)
		TSubclassOf<class ASurvivalElement> SurvivalElementActorClass;
	ASurvivalElement* SurvivalElementActor;

	bool isInteractable;
};
