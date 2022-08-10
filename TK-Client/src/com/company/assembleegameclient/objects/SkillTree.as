package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.Panel;

import kabam.rotmg.NewSkillTree.components.NewSkillTreePanel;

public class SkillTree extends GameObject implements IInteractiveObject {

    public function SkillTree(xml:XML) {
        super(xml);
        this.isInteractive_ = true;
    }

    public function getPanel(gs:GameSprite):Panel {
        return new NewSkillTreePanel(gs);
    }
}
}
