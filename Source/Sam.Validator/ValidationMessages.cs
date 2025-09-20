using System.Collections.Generic;
using System.Globalization;

namespace Sam.Validator
{
    internal class ValidationMessages
    {
        public const string ValueCannotBeNull = "ValueCannotBeNull";
        public const string ValueCannotBeEmpty = "ValueCannotBeEmpty";
        public const string InvalidEmail = "InvalidEmail";
        public const string PatternMismatch = "PatternMismatch";
        public const string MustBeOneOf = "MustBeOneOf";
        public const string MinimumValue = "MinimumValue";
        public const string MaximumValue = "MaximumValue";
        public const string LengthRange = "LengthRange";
        public const string GreaterThan = "GreaterThan";
        public const string LessThan = "LessThan";
        public const string CustomConditionFailed = "CustomConditionFailed";
        public static string Get(string key, params object[] args)
        {
            var culture = CultureInfo.CurrentUICulture.Name.Split("-")[0].ToLower();

            if (ValidationMessages.Messages.TryGetValue(key, out Dictionary<string, string>? translations))
            {
                if (translations.TryGetValue(culture, out var message))
                {
                    return string.Format(message, args);
                }
                else
                {
                    if (translations.TryGetValue("en", out message))
                    {
                        return string.Format(message, args);
                    }
                }

            }

            return key;
        }

        public static Dictionary<string, Dictionary<string, string>> Messages = new Dictionary<string, Dictionary<string, string>>()
        {
            ["ValueCannotBeNull"] = new Dictionary<string, string>()
            {
                ["en"] = "Value cannot be null.",
                ["fa"] = "مقدار نمی‌تواند تهی باشد.",
                ["ru"] = "Значение не может быть null.",
                ["de"] = "Wert darf nicht null sein.",
                ["fr"] = "La valeur ne peut pas être nulle.",
                ["es"] = "El valor no puede ser nulo.",
                ["zh"] = "值不能为空。",
                ["ja"] = "値を null にすることはできません。",
                ["it"] = "Il valore non può essere nullo.",
                ["tr"] = "Değer null olamaz.",
                ["ko"] = "값은 null일 수 없습니다.",
                ["hi"] = "मान शून्य नहीं हो सकता।"
            },
            ["ValueCannotBeEmpty"] = new Dictionary<string, string>()
            {
                ["en"] = "Value cannot be empty.",
                ["fa"] = "مقدار نمی‌تواند خالی باشد.",
                ["ru"] = "Значение не может быть пустым.",
                ["de"] = "Wert darf nicht leer sein.",
                ["fr"] = "La valeur ne peut pas être vide.",
                ["es"] = "El valor no puede estar vacío.",
                ["zh"] = "值不能为空。",
                ["ja"] = "値を空にすることはできません。",
                ["it"] = "Il valore non può essere vuoto.",
                ["tr"] = "Değer boş olamaz.",
                ["ko"] = "값은 비워둘 수 없습니다.",
                ["hi"] = "मान खाली नहीं हो सकता।"
            },
            ["InvalidEmail"] = new Dictionary<string, string>()
            {
                ["en"] = "Invalid email format.",
                ["fa"] = "فرمت ایمیل نامعتبر است.",
                ["ru"] = "Неверный формат электронной почты.",
                ["de"] = "Ungültiges E-Mail-Format.",
                ["fr"] = "Format d'e-mail invalide.",
                ["es"] = "Formato de correo electrónico no válido.",
                ["zh"] = "无效的电子邮件格式。",
                ["ja"] = "無効なメール形式です。",
                ["it"] = "Formato email non valido.",
                ["tr"] = "Geçersiz e-posta formatı.",
                ["ko"] = "유효하지 않은 이메일 형식입니다.",
                ["hi"] = "अमान्य ईमेल प्रारूप।"
            },
            ["PatternMismatch"] = new Dictionary<string, string>()
            {
                ["en"] = "Value does not match the required pattern.",
                ["fa"] = "مقدار با الگوی مورد نظر مطابقت ندارد.",
                ["ru"] = "Значение не соответствует требуемому шаблону.",
                ["de"] = "Wert entspricht nicht dem erforderlichen Muster.",
                ["fr"] = "La valeur ne correspond pas au modèle requis.",
                ["es"] = "El valor no coincide con el patrón requerido.",
                ["zh"] = "值不符合所需的模式。",
                ["ja"] = "値が必要なパターンと一致しません。",
                ["it"] = "Il valore non corrisponde al modello richiesto.",
                ["tr"] = "Değer gerekli desene uymuyor.",
                ["ko"] = "값이 요구되는 형식과 일치하지 않습니다.",
                ["hi"] = "मान आवश्यक पैटर्न से मेल नहीं खाता।"
            },
            ["MustBeOneOf"] = new Dictionary<string, string>()
            {
                ["en"] = "Value must be one of: {0}",
                ["fa"] = "مقدار باید یکی از موارد زیر باشد: {0}",
                ["ru"] = "Значение должно быть одним из: {0}",
                ["de"] = "Wert muss einer der folgenden sein: {0}",
                ["fr"] = "La valeur doit être l'une des suivantes : {0}",
                ["es"] = "El valor debe ser uno de: {0}",
                ["zh"] = "值必须是以下之一：{0}",
                ["ja"] = "値は次のいずれかでなければなりません: {0}",
                ["it"] = "Il valore deve essere uno di: {0}",
                ["tr"] = "Değer şu seçeneklerden biri olmalı: {0}",
                ["ko"] = "값은 다음 중 하나여야 합니다: {0}",
                ["hi"] = "मान निम्न में से एक होना चाहिए: {0}"
            },
            ["MinimumValue"] = new Dictionary<string, string>()
            {
                ["en"] = "Minimum allowed value is {0}.",
                ["fa"] = "حداقل مقدار مجاز {0} است.",
                ["ru"] = "Минимально допустимое значение: {0}",
                ["de"] = "Minimal zulässiger Wert ist {0}.",
                ["fr"] = "La valeur minimale autorisée est {0}.",
                ["es"] = "El valor mínimo permitido es {0}.",
                ["zh"] = "允许的最小值是 {0}。",
                ["ja"] = "許容される最小値は {0} です。",
                ["it"] = "Il valore minimo consentito è {0}.",
                ["tr"] = "İzin verilen minimum değer: {0}",
                ["ko"] = "허용되는 최소값은 {0}입니다.",
                ["hi"] = "अनुमत न्यूनतम मान {0} है।"
            },
            ["MaximumValue"] = new Dictionary<string, string>()
            {
                ["en"] = "Maximum allowed value is {0}.",
                ["fa"] = "حداکثر مقدار مجاز {0} است.",
                ["ru"] = "Максимально допустимое значение: {0}",
                ["de"] = "Maximal zulässiger Wert ist {0}.",
                ["fr"] = "La valeur maximale autorisée est {0}.",
                ["es"] = "El valor máximo permitido es {0}.",
                ["zh"] = "允许的最大值是 {0}。",
                ["ja"] = "許容される最大値は {0} です。",
                ["it"] = "Il valore massimo consentito è {0}.",
                ["tr"] = "İzin verilen maksimum değer: {0}",
                ["ko"] = "허용되는 최대값은 {0}입니다.",
                ["hi"] = "अनुमत अधिकतम मान {0} है।"
            },
            ["LengthRange"] = new Dictionary<string, string>()
            {
                ["en"] = "Value must be between {0} and {1} characters.",
                ["fa"] = "مقدار باید بین {0} تا {1} نویسه باشد.",
                ["ru"] = "Значение должно быть от {0} до {1} символов.",
                ["de"] = "Wert muss zwischen {0} und {1} Zeichen lang sein.",
                ["fr"] = "La valeur doit comporter entre {0} et {1} caractères.",
                ["es"] = "El valor debe tener entre {0} y {1} caracteres.",
                ["zh"] = "值的长度必须在 {0} 到 {1} 个字符之间。",
                ["ja"] = "値は {0} から {1} 文字の間でなければなりません。",
                ["it"] = "Il valore deve essere compreso tra {0} e {1} caratteri.",
                ["tr"] = "Değer {0} ile {1} karakter arasında olmalıdır.",
                ["ko"] = "값은 {0}자에서 {1}자 사이여야 합니다.",
                ["hi"] = "मान {0} से {1} वर्णों के बीच होना चाहिए।"
            },
            ["GreaterThan"] = new Dictionary<string, string>()
            {
                ["en"] = "Value must be greater than {0}.",
                ["fa"] = "مقدار باید بیشتر از {0} باشد.",
                ["ru"] = "Значение должно быть больше чем {0}.",
                ["de"] = "Wert muss größer als {0} sein.",
                ["fr"] = "La valeur doit être supérieure à {0}.",
                ["es"] = "El valor debe ser mayor que {0}.",
                ["zh"] = "值必须大于 {0}。",
                ["ja"] = "値は {0} より大きくなければなりません。",
                ["it"] = "Il valore deve essere maggiore di {0}.",
                ["tr"] = "Değer {0}'dan büyük olmalıdır.",
                ["ko"] = "값은 {0}보다 커야 합니다.",
                ["hi"] = "मान {0} से अधिक होना चाहिए।"
            },
            ["LessThan"] = new Dictionary<string, string>()
            {
                ["en"] = "Value must be less than {0}.",
                ["fa"] = "مقدار باید کمتر از {0} باشد.",
                ["ru"] = "Значение должно быть меньше чем {0}.",
                ["de"] = "Wert muss kleiner als {0} sein.",
                ["fr"] = "La valeur doit être inférieure à {0}.",
                ["es"] = "El valor debe ser menor que {0}.",
                ["zh"] = "值必须小于 {0}。",
                ["ja"] = "値は {0} より小さくなければなりません。",
                ["it"] = "Il valore deve essere inferiore a {0}.",
                ["tr"] = "Değer {0}'dan küçük olmalıdır.",
                ["ko"] = "값은 {0}보다 작아야 합니다.",
                ["hi"] = "मान {0} से कम होना चाहिए।"
            },
            ["CustomConditionFailed"] = new Dictionary<string, string>()
            {
                ["en"] = "Custom validation failed.",
                ["fa"] = "اعتبارسنجی سفارشی انجام نشد.",
                ["ru"] = "Пользовательская проверка не удалась.",
                ["de"] = "Benutzerdefinierte Validierung fehlgeschlagen.",
                ["fr"] = "Échec de la validation personnalisée.",
                ["es"] = "Falló la validación personalizada.",
                ["zh"] = "自定义验证失败。",
                ["ja"] = "カスタム検証に失敗しました。",
                ["it"] = "La validazione personalizzata non è riuscita.",
                ["tr"] = "Özel doğrulama başarısız oldu.",
                ["ko"] = "사용자 지정 유효성 검사에 실패했습니다.",
                ["hi"] = "कस्टम वैधता जाँच विफल हुई।"
            }
        };

    }
}