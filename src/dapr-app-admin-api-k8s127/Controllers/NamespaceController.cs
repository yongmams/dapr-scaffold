using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DarpApp.Admin.API.K8S127.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NamespaceController : ControllerBase
    {
        private IDictionary<string, string> DefaultLabels = new Dictionary<string, string>()
        {
            {"dapr-admin/owner","dapr-admin"}
        };

        private readonly ILogger<NamespaceController> _logger;
        private readonly IKubernetes _client;

        public NamespaceController(
            ILogger<NamespaceController> logger,
            IKubernetes client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            V1NamespaceList namespaces = await _client.CoreV1.ListNamespaceAsync(labelSelector: ConvertLabelSelector(DefaultLabels));
            var result = new List<string>();

            foreach (var ns in namespaces.Items)
            {
                result.Add(ns.Metadata.Name);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] string name)
        {
            V1Namespace ns = new V1Namespace()
            {
                Metadata = new V1ObjectMeta()
                {
                    Name = name,
                    Labels = DefaultLabels
                }
            };

            await _client.CoreV1.CreateNamespaceAsync(ns);

            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            V1Namespace ns;
            try
            {
                ns = await _client.CoreV1.ReadNamespaceAsync(name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (!ContainsDictionary(ns.Labels(), DefaultLabels))
            {
                return BadRequest("this namespace owner is not dapr admin.");
            }

            V1DeleteOptions options = new V1DeleteOptions();
            _client.CoreV1.DeleteNamespace(name, options);

            return Ok();
        }

        private static bool ContainsDictionary(IDictionary<string, string> origin, IDictionary<string, string> target)
        {
            if (origin == null)
            {
                return false;
            }

            return target.All(pair => origin.ContainsKey(pair.Key) && origin[pair.Key] == pair.Value);
        }

        private static string ConvertLabelSelector(IDictionary<string, string> labels)
        {
            var result = new StringBuilder();
            foreach (var item in labels)
            {
                if (result.Length > 0)
                {
                    result.Append(",");
                }
                result.Append($"{item.Key}={item.Value}");
            }
            return result.ToString();
        }
    }
}