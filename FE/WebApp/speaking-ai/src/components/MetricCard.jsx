import React from "react";

export const MetricCard = ({ title, value, change, color }) => {
  const getChangeColor = (changeValue) => {
    const numericChange = parseFloat(changeValue);
    return numericChange >= 0 ? "text-green-500" : "text-red-500";
  };

  return (
    <div className={`p-6 rounded-lg shadow-sm bg-white dark:bg-gray-800`}>
      <h3 className="text-gray-500 dark:text-gray-400 text-sm font-medium">
        {title}
      </h3>

      <div className="mt-2 flex items-baseline">
        <span className="text-2xl font-semibold text-gray-900 dark:text-white">
          {value}
        </span>
      </div>

      <div className={`mt-2 ${getChangeColor(change)}`}>
        <span className="text-sm font-medium">{change} from last month</span>
      </div>
    </div>
  );
};
