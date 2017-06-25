using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace TranslateApp
{
	public class AzureManager
	{

		private static AzureManager instance;
		private MobileServiceClient client;
		private IMobileServiceTable<Translate2Model> Translate2Table;

		private AzureManager()
		{
			this.client = new MobileServiceClient("https://module2translate.azurewebsites.net");
            this.Translate2Table = this.client.GetTable<Translate2Model>();
		}

		public MobileServiceClient AzureClient
		{
			get { return client; }
		}

		public static AzureManager AzureManagerInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new AzureManager();
				}

				return instance;
			}
		}

		public async Task<List<Translate2Model>> GetTranslate2Information()
		{
			return await this.Translate2Table.ToListAsync();
		}

        public async Task PostTranslate2Information(Translate2Model Translate2Model)
		{
			await this.Translate2Table.InsertAsync(Translate2Model);
		}
	}
}
