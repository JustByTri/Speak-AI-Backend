import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import BookingDashboard from "./pages/BookingDashboard";
import LoginPage from "./pages/LoginPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<LoginPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/dashboard" element={<BookingDashboard />} />
      </Routes>
    </Router>
  );
}

export default App;
