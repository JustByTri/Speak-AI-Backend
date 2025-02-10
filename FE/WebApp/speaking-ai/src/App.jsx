import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import BookingDashboard from "./pages/BookingDashboard";
import LoginPage from "./pages/LoginPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/dashboard" element={<BookingDashboard />} />
        {/* Thêm các routes khác nếu cần */}
        <Route path="/" element={<BookingDashboard />} />{" "}
        {/* Redirect trang chủ đến dashboard */}
        <Route path="/login" element={<LoginPage />} />{" "}
      </Routes>
    </Router>
  );
}

export default App;
