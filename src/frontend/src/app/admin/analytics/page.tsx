// src/app/admin/analytics/page.tsx

export default function AnalyticsPage() {
    return (
      <div className="grid gap-6">
        <h1 className="text-2xl font-bold tracking-tight">Analytics</h1>
        
        {/* Analytics Cards */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="bg-white p-6 rounded-lg shadow">
            <h3 className="text-lg font-medium text-gray-900">Sentiment Overview</h3>
            <div className="mt-4 h-48 bg-gray-50 rounded flex items-center justify-center">
              Chart Placeholder
            </div>
          </div>
          
          <div className="bg-white p-6 rounded-lg shadow">
            <h3 className="text-lg font-medium text-gray-900">Review Trends</h3>
            <div className="mt-4 h-48 bg-gray-50 rounded flex items-center justify-center">
              Chart Placeholder
            </div>
          </div>
          
          <div className="bg-white p-6 rounded-lg shadow">
            <h3 className="text-lg font-medium text-gray-900">Top Keywords</h3>
            <div className="mt-4 h-48 bg-gray-50 rounded flex items-center justify-center">
              Chart Placeholder
            </div>
          </div>
          
          <div className="bg-white p-6 rounded-lg shadow">
            <h3 className="text-lg font-medium text-gray-900">Theme Analysis</h3>
            <div className="mt-4 h-48 bg-gray-50 rounded flex items-center justify-center">
              Chart Placeholder
            </div>
          </div>
        </div>
      </div>
    );
  }