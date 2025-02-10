import React from "react";
import Sidebar from "../components/layout/Sidebar";
import Header from "../components/layout/Header";
import BookingStats from "../components/dashboard/BookingStats";
import BookingCharts from "../components/dashboard/BookingCharts";
import RecentBookings from "../components/dashboard/RecentBookings";

const BookingDashboard = () => {
  return (
    <div className="flex bg-gray-100 min-h-screen">
      <Sidebar />
      <div className="flex-1 ml-64">
        <Header />
        <main className="p-6">
          <BookingStats />
          <BookingCharts />
          <RecentBookings />
        </main>
      </div>
    </div>
  );
};

export default BookingDashboard;
