using System;
using TagExtractor;

namespace Runner
{
	class Program
	{
		private static string _defaultApiKey = "029352e2d404450db4394cd4e3758105";
		private static string _defaultUrl = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/";

		static void Main(string[] args) {
			var tExtractor = new TagExtractorExecuter(_defaultUrl, _defaultApiKey, s => s.Equals(_defaultApiKey) );
			try {
				tExtractor.GetTags(new Guid(), @"The entire Pro Git book, written by Scott Chacon and Ben Straub and published by Apress, is available here.
				All content is licensed under the Creative Commons Attribution Non Commercial Share Alike 3.0 license. 
				Print versions of the book are available on Amazon.com.").ForEach(Console.WriteLine);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
			try {
				var	tags = tExtractor.GetTags(new Guid(), @"Что то на русском языке Табло Важно Демо");
				tags.ForEach(Console.WriteLine);
			} catch (UnsupportedLangException exception) {
				Console.WriteLine(exception.Message);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
	}
}
