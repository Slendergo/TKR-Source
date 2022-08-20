package kabam.rotmg.Engine
{
import com.company.assembleegameclient.account.ui.ChooseNameFrame;
import com.company.assembleegameclient.account.ui.ChooseNameFrameMediator;
import com.company.assembleegameclient.account.ui.NewChooseNameFrame;
import com.company.assembleegameclient.account.ui.NewChooseNameFrameMediator;
import com.company.assembleegameclient.mapeditor.MapEditor;
import com.company.assembleegameclient.screens.AccountScreen;
import com.company.assembleegameclient.screens.CharacterSelectionAndNewsScreen;
import com.company.assembleegameclient.screens.CreditsScreen;
import com.company.assembleegameclient.screens.GraveyardLine;
import com.company.assembleegameclient.screens.LoadingScreen;
import com.company.assembleegameclient.screens.NewCharacterScreen;
import com.company.assembleegameclient.screens.ServersScreen;
import com.company.assembleegameclient.screens.charrects.CharacterRectList;
import com.company.assembleegameclient.screens.charrects.CurrentCharacterRect;
import com.company.assembleegameclient.ui.GameObjectStatusPanel;
import com.company.assembleegameclient.ui.dialogs.ErrorDialog;
import com.company.assembleegameclient.ui.menu.PlayerGroupMenu;
import com.company.assembleegameclient.ui.panels.InteractPanel;
import com.company.assembleegameclient.ui.panels.itemgrids.ItemGrid;
import com.company.assembleegameclient.ui.panels.mediators.InteractPanelMediator;
import com.company.assembleegameclient.ui.panels.mediators.ItemGridMediator;

import io.decagames.rotmg.ui.popups.PopupMediator;
import io.decagames.rotmg.ui.popups.PopupView;
import io.decagames.rotmg.ui.popups.modal.ConfirmationModal;
import io.decagames.rotmg.ui.popups.modal.ConfirmationModalMediator;
import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.popups.modal.ModalPopupMediator;
import io.decagames.rotmg.ui.popups.modal.buttons.BuyGoldButton;
import io.decagames.rotmg.ui.popups.modal.buttons.BuyGoldButtonMediator;
import io.decagames.rotmg.ui.popups.modal.buttons.CancelButtonMediator;
import io.decagames.rotmg.ui.popups.modal.buttons.ClosePopupButton;
import io.decagames.rotmg.ui.popups.signals.ClosePopupSignal;
import io.decagames.rotmg.ui.scroll.UIScrollbar;
import io.decagames.rotmg.ui.scroll.UIScrollbarMediator;
import io.decagames.rotmg.ui.spinner.NumberSpinner;
import io.decagames.rotmg.ui.spinner.NumberSpinnerMediator;
import io.decagames.rotmg.ui.tabs.UITab;
import io.decagames.rotmg.ui.tabs.UITabMediator;

import kabam.rotmg.Engine.view.EngineView;
import kabam.rotmg.Engine.view.EngineViewMediator;

import kabam.rotmg.Forge.ForgeModal;
import kabam.rotmg.Forge.ForgeModalMediator;
import kabam.rotmg.account.core.services.GetCharListTask;
import kabam.rotmg.account.core.services.LoadAccountTask;
import kabam.rotmg.account.core.view.AccountInfoMediator;
import kabam.rotmg.account.core.view.AccountInfoView;
import kabam.rotmg.account.core.view.RegisterPromptDialog;
import kabam.rotmg.account.core.view.RegisterPromptDialogMediator;
import kabam.rotmg.application.api.ApplicationSetup;
import kabam.rotmg.assets.task.GetServerXmlsTask;
import kabam.rotmg.essences.view.EssenceView;
import kabam.rotmg.essences.view.EssenceViewMediator;
import kabam.rotmg.game.model.PotionInventoryModel;
import kabam.rotmg.game.view.NameChangerPanel;
import kabam.rotmg.game.view.NameChangerPanelMediator;
import kabam.rotmg.startup.control.StartupSequence;
import kabam.rotmg.ui.commands.ChooseNameCommand;
import kabam.rotmg.ui.commands.EnterGameCommand;
import kabam.rotmg.ui.commands.HUDInitCommand;
import kabam.rotmg.ui.commands.RefreshScreenAfterLoginCommand;
import kabam.rotmg.ui.commands.ShowEternalPopUICommand;
import kabam.rotmg.ui.commands.ShowKeyUICommand;
import kabam.rotmg.ui.commands.ShowLegendaryPopUICommand;
import kabam.rotmg.ui.commands.ShowLoadingUICommand;
import kabam.rotmg.ui.commands.ShowRevengePopUICommand;
import kabam.rotmg.ui.commands.ShowTitleUICommand;
import kabam.rotmg.ui.model.HUDModel;
import kabam.rotmg.ui.noservers.NoServersDialogFactory;
import kabam.rotmg.ui.noservers.ProductionNoServersDialogFactory;
import kabam.rotmg.ui.noservers.TestingNoServersDialogFactory;
import kabam.rotmg.ui.signals.ChooseNameSignal;
import kabam.rotmg.ui.signals.EnterGameSignal;
import kabam.rotmg.ui.signals.EternalPopUpSignal;
import kabam.rotmg.ui.signals.HUDModelInitialized;
import kabam.rotmg.ui.signals.HUDSetupStarted;
import kabam.rotmg.ui.signals.HideKeySignal;
import kabam.rotmg.ui.signals.LegendaryPopUpSignal;
import kabam.rotmg.ui.signals.NameChangedSignal;
import kabam.rotmg.ui.signals.RefreshScreenAfterLoginSignal;
import kabam.rotmg.ui.signals.RevengePopUpSignal;
import kabam.rotmg.ui.signals.ShowKeySignal;
import kabam.rotmg.ui.signals.ShowKeyUISignal;
import kabam.rotmg.ui.signals.ShowLoadingUISignal;
import kabam.rotmg.ui.signals.ShowTitleUISignal;
import kabam.rotmg.ui.signals.StatsTabHotKeyInputSignal;
import kabam.rotmg.ui.signals.UpdateBackpackTabSignal;
import kabam.rotmg.ui.signals.UpdateHUDSignal;
import kabam.rotmg.ui.signals.UpdatePotionInventorySignal;
import kabam.rotmg.ui.view.AccountScreenMediator;
import kabam.rotmg.ui.view.CharacterDetailsMediator;
import kabam.rotmg.ui.view.CharacterDetailsView;
import kabam.rotmg.ui.view.CharacterRectListMediator;
import kabam.rotmg.ui.view.CharacterSlotNeedGoldDialog;
import kabam.rotmg.ui.view.CharacterSlotNeedGoldMediator;
import kabam.rotmg.ui.view.CharacterSlotRegisterDialog;
import kabam.rotmg.ui.view.CharacterSlotRegisterMediator;
import kabam.rotmg.ui.view.ChooseNameRegisterDialog;
import kabam.rotmg.ui.view.ChooseNameRegisterMediator;
import kabam.rotmg.ui.view.CreditsMediator;
import kabam.rotmg.ui.view.CurrentCharacterMediator;
import kabam.rotmg.ui.view.CurrentCharacterRectMediator;
import kabam.rotmg.ui.view.ErrorDialogMediator;
import kabam.rotmg.ui.view.FlexibleDialog;
import kabam.rotmg.ui.view.FlexibleMediator;
import kabam.rotmg.ui.view.GameObjectStatusPanelMediator;
import kabam.rotmg.ui.view.HUDMediator;
import kabam.rotmg.ui.view.HUDView;
import kabam.rotmg.ui.view.KeysMediator;
import kabam.rotmg.ui.view.KeysView;
import kabam.rotmg.ui.view.LoadingMediator;
import kabam.rotmg.ui.view.MapEditorMediator;
import kabam.rotmg.ui.view.NewCharacterMediator;
import kabam.rotmg.ui.view.NewsLineMediator;
import kabam.rotmg.ui.view.NotEnoughGoldDialog;
import kabam.rotmg.ui.view.NotEnoughGoldMediator;
import kabam.rotmg.ui.view.ServersMediator;
import kabam.rotmg.ui.view.StatMetersMediator;
import kabam.rotmg.ui.view.StatMetersView;
import kabam.rotmg.ui.view.TitleMediator;
import kabam.rotmg.ui.view.TitleView;
import kabam.rotmg.ui.view.components.PotionSlotMediator;
import kabam.rotmg.ui.view.components.PotionSlotView;

import org.swiftsuspenders.Injector;

import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
import robotlegs.bender.extensions.signalCommandMap.api.ISignalCommandMap;
import robotlegs.bender.framework.api.IConfig;

public class EngineConfig implements IConfig
{
    [Inject]
    public var injector:Injector;

    [Inject]
    public var mediatorMap:IMediatorMap;

    [Inject]
    public var commandMap:ISignalCommandMap;

    [Inject]
    public var setup:ApplicationSetup;

    [Inject]
    public var startup:StartupSequence;

    public function EngineConfig()
    {
        super();
    }

    public function configure() : void
    {
        this.mediatorMap.map(EngineView).toMediator(EngineViewMediator);
    }
}
}
