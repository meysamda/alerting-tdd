namespace Alerting.Presentation.Swagger
{
    public class SwaggerOptions
    {
        public SwaggerDocOptions Doc { get; set; }
        public SwaggerOAuthOptions OAuth { get; set; }

        public class SwaggerDocOptions
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Version { get; set; }
            public SwaggerDocContactOptions Contact { get; set; }
        }
        public class SwaggerDocContactOptions
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Url { get; set; }
        }

        public class SwaggerOAuthOptions
        {
            public string AuthorizationUrl { get; set; }
            public string TokenUrl { get; set; }
            public string RefreshUrl { get; set; }
        }
    }
}
