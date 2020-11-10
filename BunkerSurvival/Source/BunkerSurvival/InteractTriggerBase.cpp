// Fill out your copyright notice in the Description page of Project Settings.


#include "InteractTriggerBase.h"
#include "DrawDebugHelpers.h"
#include "Components/BillboardComponent.h"
#include "Components/TextRenderComponent.h"
#include "Components/ShapeComponent.h"
#include "Materials/MaterialInterface.h"
#include "Materials/Material.h"
#include "Engine/Font.h"
#include "Engine/StaticMeshActor.h"
#include "Kismet/GameplayStatics.h"
#include "SurvivalElement.h"

AInteractTriggerBase::AInteractTriggerBase()
{
    //Create the static mesh.
    TriggerMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Trigger Mesh"));
    TriggerMesh->SetupAttachment(RootComponent);

    //Create and configure the TextBillBoard.
    TriggerBillboardTextRender = CreateDefaultSubobject<UTextRenderComponent>(TEXT("Trigger Billboard Text"));
    TriggerBillboardTextRender->SetText(TEXT("press E"));
    TriggerBillboardTextRender->SetupAttachment(RootComponent);
    TriggerBillboardTextRender->HorizontalAlignment = EHorizTextAligment::EHTA_Center;
    TriggerBillboardTextRender->VerticalAlignment = EVerticalTextAligment::EVRTA_TextCenter;
    TriggerBillboardTextRender->SetUsingAbsoluteScale(true);
    TriggerBillboardTextRender->SetRelativeLocation(FVector(0.0f, 0.0f, 50.0f));
    static ConstructorHelpers::FObjectFinder<UFont> FontClass(
        TEXT("Font'/Game/BluePrints/TriggerFont.TriggerFont'"));
    if (FontClass.Object)
        TriggerBillboardTextRender->SetFont(FontClass.Object);
    static ConstructorHelpers::FObjectFinder<UMaterialInterface> Material(
        TEXT("Material'/Game/BluePrints/TriggerFontMaterial.TriggerFontMaterial'"));
    if (Material.Object)
        TriggerBillboardTextRender->SetMaterial(0, Material.Object);

    SetActorHiddenInGame(false);
    GetCollisionComponent()->SetHiddenInGame(true);
    //TextBillBoard is only displayed while player is inside the trigger.
    TriggerBillboardTextRender->SetHiddenInGame(true);

    isInteractable = false;
}

void AInteractTriggerBase::BeginPlay()
{
    Super::BeginPlay();

    //Register Events
    OnActorBeginOverlap.AddDynamic(this, &AInteractTriggerBase::OnOverlapBegin);
    OnActorEndOverlap.AddDynamic(this, &AInteractTriggerBase::OnOverlapEnd);
    if(SurvivalElementActorClass)
        SurvivalElementActor = Cast<ASurvivalElement>(UGameplayStatics::GetActorOfClass(GetWorld(), SurvivalElementActorClass));

    //Disable the glow effect by default. Only the active trigger have this effect.
    //It is managed inside Survival Element actor.
    TriggerMesh->SetScalarParameterValueOnMaterials("FresnelMultiplier", 0.0);
}

void AInteractTriggerBase::OnOverlapBegin(class AActor* OverlappedActor, class AActor* OtherActor)
{
    // check if Actors do not equal nullptr and isInteractable.
    if (OtherActor && (OtherActor != this) && isInteractable) {
        //Used previously to interact with last entered trigger.
        //Later changed the idea to activate only one trigger at a time.
        /*if (SurvivalElementActor->CurrentTrigger != nullptr)
            SurvivalElementActor->CurrentTrigger->OnActorEndOverlap.Broadcast(SurvivalElementActor->CurrentTrigger, OtherActor);*/
        SurvivalElementActor->CurrentTrigger = this;
        //Display TextBillboard.
        TriggerBillboardTextRender->SetHiddenInGame(false);

        //Bind Interaction input to Interact()
        EnableInput(UGameplayStatics::GetPlayerController(GetWorld(), 0));
        if (InputComponent)
        {
            InputComponent->BindAction("Interact", IE_Pressed, this, &AInteractTriggerBase::Interact);
        }
    }
}

void AInteractTriggerBase::OnOverlapEnd(class AActor* OverlappedActor, class AActor* OtherActor)
{
    // check if Actors do not equal nullptr and isInteractable.
    if (OtherActor && (OtherActor != this) && isInteractable) {
        //Hide TextBillboard.
        TriggerBillboardTextRender->SetHiddenInGame(true);
        //Used previously to enable only one trigger at a time.
        /*
        if (SurvivalElementActor->CurrentTrigger == this)
        {
            SurvivalElementActor->CurrentTrigger = nullptr;
        }*/
    }
}

void AInteractTriggerBase::Interact()
{
    if (isInteractable)
    {
        //Increasse the element's currentlevel and deactivate the trigger.
        SurvivalElementActor->CurrentLevel += 0.2f;
        SurvivalElementActor->CurrentLevel = FMath::Clamp(SurvivalElementActor->CurrentLevel, 0.0f, 1.0f);
        GetCollisionComponent()->DestroyComponent();
        TriggerBillboardTextRender->DestroyComponent();
        TriggerMesh->SetScalarParameterValueOnMaterials("FresnelMultiplier", 0.0);
        isInteractable = false;
        //Enable next trigger.
        SurvivalElementActor->EnableNextTrigger();
    }
}
