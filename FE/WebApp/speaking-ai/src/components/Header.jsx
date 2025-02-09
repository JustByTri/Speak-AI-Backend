import React from "react";
import { FiMenu, FiBell, FiSun, FiMoon } from "react-icons/fi";
import { SearchBar } from "./SearchBar";

export const Header = ({
  sidebarOpen,
  setSidebarOpen,
  darkMode,
  setDarkMode,
}) => {
  return (
    <header className="bg-white dark:bg-gray-800 border-b dark:border-gray-700">
      <div className="px-4 py-4 lg:px-6">
        <div className="flex items-center justify-between">
          <button
            onClick={() => setSidebarOpen(!sidebarOpen)}
            className="lg:hidden p-2 rounded-lg text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700 transition-colors"
          >
            <FiMenu className="h-6 w-6" />
          </button>

          <div className="flex-1 flex items-center justify-end space-x-4">
            <SearchBar />

            <div className="flex items-center space-x-2">
              <button className="p-2 rounded-lg text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700 transition-colors">
                <FiBell className="h-6 w-6" />
              </button>

              <button
                onClick={() => setDarkMode(!darkMode)}
                className="p-2 rounded-lg text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700 transition-colors"
              >
                {darkMode ? (
                  <FiSun className="h-6 w-6" />
                ) : (
                  <FiMoon className="h-6 w-6" />
                )}
              </button>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
};
