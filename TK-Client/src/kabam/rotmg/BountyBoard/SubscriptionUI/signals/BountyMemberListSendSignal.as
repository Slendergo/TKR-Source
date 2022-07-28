package kabam.rotmg.BountyBoard.SubscriptionUI.signals {

import kabam.rotmg.messaging.impl.incoming.bounty.BountyMemberListSend;

import org.osflash.signals.Signal;

public class BountyMemberListSendSignal extends Signal
{
    public static var instance:BountyMemberListSendSignal;

    public function BountyMemberListSendSignal()
    {
        super(BountyMemberListSend);
        instance = this;
    }
}
}
