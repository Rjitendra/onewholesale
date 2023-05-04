namespace OneWholeSale.API.Policies
{
    public static class AuthPolicyNames
    {
        /// <summary>
		/// Require user to have at least 1 role
		/// </summary>
		public const string HasRole = "HasRole";

        /// <summary>
        /// Require user is a System Admin
        /// </summary>
        //public const string IsSysAdmin = "IsSysAdmin";
    }
}