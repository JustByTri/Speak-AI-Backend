import React, { useState } from "react";
import { Chart as ChartJS } from "chart.js/auto";
import { MainLayout } from "../../layouts/MainLayout";
import { MetricsGrid } from "../../components/MetricsGrid";
import { ChartSection } from "../../components/ChartSection";
import { RecentActivity } from "../../components/RecentActivity";

const AdminDashboard = () => {
  const [sidebarOpen, setSidebarOpen] = useState(true);
  const [darkMode, setDarkMode] = useState(false);
  const [activeTab, setActiveTab] = useState("Dashboard");

  const metrics = [
    {
      title: "Total Users",
      value: "24,521",
      change: "+12.5%",
      color: "bg-blue-500",
    },
    {
      title: "Revenue",
      value: "$86,249",
      change: "+8.2%",
      color: "bg-green-500",
    },
    {
      title: "Active Projects",
      value: "156",
      change: "-2.4%",
      color: "bg-purple-500",
    },
    {
      title: "Pending Tasks",
      value: "38",
      change: "+5.6%",
      color: "bg-yellow-500",
    },
  ];

  const recentActivity = [
    { user: "John Doe", action: "Created new project", time: "2 hours ago" },
    { user: "Jane Smith", action: "Updated user profile", time: "4 hours ago" },
    {
      user: "Mike Johnson",
      action: "Completed task #127",
      time: "6 hours ago",
    },
  ];

  const lineChartData = {
    labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
    datasets: [
      {
        label: "Monthly Performance",
        data: [65, 59, 80, 81, 56, 55],
        fill: false,
        borderColor: "rgb(75, 192, 192)",
        tension: 0.1,
      },
    ],
  };

  const pieChartData = {
    labels: ["Development", "Marketing", "Sales", "Support"],
    datasets: [
      {
        data: [30, 25, 25, 20],
        backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0"],
      },
    ],
  };

  return (
    <MainLayout>
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
        <div className="flex flex-col flex-1">
          <main className="flex-1 p-6">
            <div className="mb-8">
              <MetricsGrid metrics={metrics} />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-8">
              <ChartSection
                lineChartData={lineChartData}
                pieChartData={pieChartData}
              />
            </div>

            <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
              <RecentActivity activities={recentActivity} />
            </div>
          </main>
        </div>
      </div>
    </MainLayout>
  );
};

export default AdminDashboard;
