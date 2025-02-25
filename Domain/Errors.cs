namespace Domain.Common
{
    public static class ErrorCodes
    {
        public const string ProductNotFound = "PRODUCT_NOT_FOUND";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string ProductDataIsRequired = "PRODUCT_DATA_IS_REQUIRED";
        public const string CategoryDoesNotExist = "CATEGORY_DOES_NOT_EXIST";
        public const string ProductNameIsRequired = "PRODUCT_NAME_IS_REQUIRED";
        public const string ProductNameShort = "NAME_IS_SHORTER_THAN_2";
        public const string PriceIsLow = "PRICE_IS_LOW";
        public const string NegativeQunatity = "QUANTITY_CANNOT_BE_NEGATIVE";
    }
}
