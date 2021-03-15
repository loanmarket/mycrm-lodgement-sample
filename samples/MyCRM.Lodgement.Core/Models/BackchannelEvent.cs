namespace MyCRM.Lodgement.Common.Models
{
    public enum BackchannelEvent
    {
        ApplicationSent,
        ApplicationRegistered,
        PreApproved,
        ConditionallyApproved,
        UnconditionallyApproved,
        ApplicationSettled,
        ReferredToAssessor,
        RevertedToCapture,
        Cancelled,
        Declined,
        Withdrawn
    }
}
