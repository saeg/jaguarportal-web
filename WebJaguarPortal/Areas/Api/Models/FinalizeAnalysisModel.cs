using System.ComponentModel;

namespace WebJaguarPortal.Areas.Api.Models
{
    public class FinalizeAnalysisModel
    {
        /// <summary>
        /// Message error is required if status is Error.
        /// </summary>
        public string? MessageError { get; set; }

        /// <summary>
        /// Define 0 to Success and 1 to Error.
        /// </summary>
        public StatusFinalize Status { get; set; }

        /// <summary>
        /// Define 0 to Error and 1 to Success.
        /// </summary>
        public enum StatusFinalize
        {
            /// <summary>
            /// Error
            /// </summary>
            [Description("Error")]
            [DefaultValue("E")]
            Error,
            /// <summary>
            /// Success
            /// </summary>
            [Description("Success")]
            [DefaultValue("S")]
            Success,
        }
    }
}
