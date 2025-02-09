import React from "react";
import { ActivityItem } from "./ActivityItem";

export const RecentActivity = ({ activities }) => {
  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
      <div className="p-6">
        <h2 className="text-xl font-semibold mb-4">Recent Activity</h2>

        <div className="divide-y dark:divide-gray-700">
          {activities.map((activity, index) => (
            <ActivityItem key={index} {...activity} />
          ))}
        </div>
      </div>
    </div>
  );
};
