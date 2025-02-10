import { useState } from "react";
import { loginUser } from "../../services/authService";

export const useAuth = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const login = async (credentials) => {
    try {
      setLoading(true);
      setError("");
      const data = await loginUser(credentials);
      // Handle successful login (e.g., store token, redirect)
      localStorage.setItem("token", data.token);
      return data;
    } catch (err) {
      setError("Login failed. Please try again.");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { login, loading, error };
};
