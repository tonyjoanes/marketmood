// src/app/admin/settings/page.tsx

export default function SettingsPage() {
    return (
      <div className="grid gap-6">
        <h1 className="text-2xl font-bold tracking-tight">Settings</h1>
        
        <div className="bg-white shadow rounded-lg">
          <div className="p-6">
            <h2 className="text-lg font-medium text-gray-900">Scraping Configuration</h2>
            <div className="mt-4 grid gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Scraping Interval (hours)
                </label>
                <input 
                  type="number" 
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
                  placeholder="24"
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Reviews per Product
                </label>
                <input 
                  type="number" 
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
                  placeholder="100"
                />
              </div>
              
              <div className="pt-4">
                <button className="px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-lg hover:bg-blue-700">
                  Save Settings
                </button>
              </div>
            </div>
          </div>
          
          <div className="border-t border-gray-200 p-6">
            <h2 className="text-lg font-medium text-gray-900">API Configuration</h2>
            <div className="mt-4 grid gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  API Key
                </label>
                <input 
                  type="password" 
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
                  placeholder="••••••••"
                />
              </div>
              
              <div className="pt-4">
                <button className="px-4 py-2 text-sm font-medium text-white bg-blue-600 rounded-lg hover:bg-blue-700">
                  Generate New Key
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }