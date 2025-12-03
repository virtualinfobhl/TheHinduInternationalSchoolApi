namespace ApiProject.Models.Response
{
    public class SuccessVM
    {
        public string status { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        public bool displayMessage { get; set; }
    }

    public class ErrorVM
    {
        public string? status { get; set; }
        public object? data { get; set; }
        public string? error { get; set; }
        public string? errorDetail { get; set; }
        public string? message { get; set; }

        public bool displayMessage { get; set; }
    }
}
