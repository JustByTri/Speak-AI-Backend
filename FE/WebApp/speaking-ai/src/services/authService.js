// const API_URL = process.env.REACT_APP_API_URL
const API_URL = "http://localhost:3000/api";
export const loginUser = async (credentials) => {
  try {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(credentials),
    });

    if (!response.ok) {
      throw new Error("Login failed");
    }

    const data = await response.json();
    return data;
  } catch (error) {
    throw new Error(error.message || "An error occurred during login");
  }
};
