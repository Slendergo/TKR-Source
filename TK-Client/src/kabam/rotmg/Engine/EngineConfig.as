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
import kabam.rotmg.startup.control.StartupSequence;

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
