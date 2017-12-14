using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace TagExtractor
{
	public class TagExtractorExecuter
	{
		private const string DemoKey = "78fd64a2f2474316a341ead6ce0d198e";

		private readonly ILangExtractor _langExtractor;
		private readonly IKeyPhrasesExtractor _keyPhrasesExtractor;
		private Predicate<string> _checkLic;
		private readonly bool _isDemoMode;

		public TagExtractorExecuter(string baseUrl, string apiKey, Predicate<string> checkLic = null) {
			_checkLic = checkLic;
			_isDemoMode = apiKey.Equals(DemoKey);
			_langExtractor = new AzureHttpLangExtractor(baseUrl, apiKey);
			_keyPhrasesExtractor = new AzureHttpKeyPhrasesExtractor(baseUrl, apiKey);
		}

		private void CheckDemoMode() {
			if (!_isDemoMode) return;
			if (_checkLic == null) {
				throw new DemoModeException("You can't use this app in demo mode! Please register acount on https://portal.azure.com.");
			}
			if (!_checkLic(DemoKey)) {
				throw new DemoModeException("You have exceeded the number of requests in demo mode! Please register acount on https://portal.azure.com.");
			}
		}

		private async Task<string> AsyncGetLangByText(Guid id, string text) {
			return await _langExtractor.AsyncDetectLang(id, text);
		}

		public List<string> GetTags(Guid recordId, string text, string lang) {
			CheckDemoMode();
			try {
				var result = _keyPhrasesExtractor.AsyncKeyPhrases(lang, recordId, text).GetAwaiter();
				return result.GetResult();
			} catch (Exception e) {
				throw e;
			}
		}

		public List<string> GetTags(Guid recordId, string text) {
			CheckDemoMode();
			var pText = HttpUtility.HtmlEncode(text);
			var lang = AsyncGetLangByText(recordId, pText).Result;
			return GetTags(recordId, pText, lang);
		}

	}

}
