namespace BMB.Utilities
{
    [ContentProperty(nameof(Key))]
    public class TranslateUtility : IMarkupExtension<BindingBase>
    {
        public string? Key { get; set; }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
                throw new InvalidOperationException("Translation key is missing.");

            return new Binding($"[{Key}]",
                source: LocalizationUtility.Instance,
                mode: BindingMode.OneWay);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
            => ProvideValue(serviceProvider);
    }
}
