import React, { useState } from "react";
import { Sidebar } from "../components/Sidebar";
import { Header } from "../components/Header";

export const MainLayout = ({
  children,
  darkMode,
  sidebarOpen,
  setSidebarOpen,
}) => {
  const [activeTab, setActiveTab] = useState("Dashboard"); // Initialize activeTab state
  const [darkModeState, setDarkMode] = useState(darkMode || false); // Initialize dark mode state

  return (
    <div
      className={`min-h-screen ${
        darkModeState ? "dark bg-gray-900" : "bg-gray-50"
      }`}
    >
      <Sidebar
        sidebarOpen={sidebarOpen}
        setSidebarOpen={setSidebarOpen}
        activeTab={activeTab}
        setActiveTab={setActiveTab}
      />
      <div
        className={`lg:ml-64 transition-all duration-300 ${
          sidebarOpen ? "ml-64" : "ml-0"
        }`}
      >
        <Header
          sidebarOpen={sidebarOpen}
          setSidebarOpen={setSidebarOpen}
          darkMode={darkModeState}
          setDarkMode={setDarkMode} // Pass setDarkMode here
        />
        <main className="p-6">{children}</main>
      </div>
    </div>
  );
};
