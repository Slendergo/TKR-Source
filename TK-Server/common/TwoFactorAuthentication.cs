using Google.Authenticator;

namespace common
{
    public sealed class TwoFactorAuthentication
    {
        private TwoFactorAuthenticator TwoFactorAuthenticator { get; }

        public TwoFactorAuthentication() => TwoFactorAuthenticator = new TwoFactorAuthenticator();

        public string Generate(string guid)
        {
            var setupCode = TwoFactorAuthenticator.GenerateSetupCode("Spectral Rebirth", "Account Security", $"SpectralRebirth2FAKey__{guid}", true, 300);
            return $"{setupCode.QrCodeSetupImageUrl}@{setupCode.ManualEntryKey}";
        }

        public bool IsValidPin(string guid, string pin) => TwoFactorAuthenticator.ValidateTwoFactorPIN($"SpectralRebirth2FAKey__{guid}", pin);
    }
}
