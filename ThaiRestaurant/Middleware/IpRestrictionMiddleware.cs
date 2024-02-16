//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ThaiRestaurant.Middleware
//{
//    public class IpRestrictionMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<IpRestrictionMiddleware> _logger;
//        private readonly string[] _allowedIps;

//        public IpRestrictionMiddleware(RequestDelegate next, ILogger<IpRestrictionMiddleware> logger, IConfiguration configuration)
//        {
//            _next = next;
//            _logger = logger;
//            _allowedIps = configuration.GetSection("IpRestriction:AllowedIps").Get<string[]>();
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            var remoteIp = context.Connection.RemoteIpAddress.ToString();
//            var isAllowedIp = _allowedIps.Contains(remoteIp);

//            // Log the IP check result
//            _logger.LogInformation("IP check - Remote IP: {RemoteIp}, Allowed: {IsAllowedIp}", remoteIp, isAllowedIp);

//            // Proceed with the next middleware in the pipeline
//            await _next(context);
//        }
//    }
//}
