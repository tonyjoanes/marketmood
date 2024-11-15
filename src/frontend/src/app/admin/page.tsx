// src/frontend/src/app/admin/page.tsx
export default function AdminDashboard() {
    return (
      <div className="grid gap-4">
        <h1 className="text-2xl font-bold tracking-tight">Admin Dashboard</h1>
        <p className="text-gray-500">
          Welcome to the MarketMood admin dashboard. Manage your products and review analytics here.
        </p>
        
        {/* Dashboard Stats */}
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
          <div className="bg-white p-4 rounded-lg shadow">
            <h3 className="text-gray-500 text-sm font-medium">Total Products</h3>
            <p className="text-2xl font-bold">0</p>
          </div>
          <div className="bg-white p-4 rounded-lg shadow">
            <h3 className="text-gray-500 text-sm font-medium">Active Scraping Jobs</h3>
            <p className="text-2xl font-bold">0</p>
          </div>
          <div className="bg-white p-4 rounded-lg shadow">
            <h3 className="text-gray-500 text-sm font-medium">Reviews Analyzed</h3>
            <p className="text-2xl font-bold">0</p>
          </div>
          <div className="bg-white p-4 rounded-lg shadow">
            <h3 className="text-gray-500 text-sm font-medium">Average Sentiment</h3>
            <p className="text-2xl font-bold">N/A</p>
          </div>
        </div>
      </div>
    );
  }