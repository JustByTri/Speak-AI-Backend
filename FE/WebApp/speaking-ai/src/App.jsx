import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import AdminDashboard from "./pages/dashboard/AdminDashboard";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/dashboard" element={<AdminDashboard />} />
        {/* Thêm các routes khác nếu cần */}
        <Route path="/" element={<AdminDashboard />} />{" "}
        {/* Redirect trang chủ đến dashboard */}
      </Routes>
    </Router>
  );
}

export default App;
