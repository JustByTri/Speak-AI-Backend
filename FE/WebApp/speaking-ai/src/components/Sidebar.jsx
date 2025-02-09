import React from "react";
import { FiHome, FiUsers, FiPieChart, FiSettings } from "react-icons/fi";

export const Sidebar = ({ sidebarOpen, activeTab, setActiveTab }) => {
  const navItems = [
    { icon: <FiHome className="h-5 w-5" />, name: "Dashboard" },
    { icon: <FiUsers className="h-5 w-5" />, name: "User Management" },
    { icon: <FiPieChart className="h-5 w-5" />, name: "Analytics" },
    { icon: <FiSettings className="h-5 w-5" />, name: "Settings" },
  ];

  return (
    <aside
      className={`
      fixed inset-y-0 left-0 z-50 w-64 bg-white dark:bg-gray-800 transform 
      ${sidebarOpen ? "translate-x-0" : "-translate-x-full"}
      lg:translate-x-0 transition-transform duration-200 ease-in-out
    `}
    >
      <div className="h-full flex flex-col">
        <div className="flex-1 flex flex-col pt-5 pb-4 overflow-y-auto">
          <div className="flex items-center flex-shrink-0 px-4">
            <h1 className="text-xl font-bold">Admin Dashboard</h1>
          </div>

          <nav className="mt-5 flex-1 px-2 space-y-1">
            {navItems.map((item) => (
              <button
                key={item.name}
                onClick={() => setActiveTab(item.name)}
                className={`w-full flex items-center space-x-3 px-4 py-3 rounded-lg transition-colors
                  ${
                    activeTab === item.name
                      ? "bg-blue-500 text-white"
                      : "text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700"
                  }`}
              >
                {item.icon}
                <span>{item.name}</span>
              </button>
            ))}
          </nav>
        </div>
      </div>
    </aside>
  );
};
