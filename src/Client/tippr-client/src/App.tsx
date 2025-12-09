import { Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/auth/LoginPage";
import RegisterPage from "./pages/auth/RegisterPage";
import ProtectedRoute from "./components/layout/ProtectedRoute";

// En enkel Dashboard bara för att testa
const Dashboard = () => {
  return (
    <div style={{ padding: 20 }}>
      <h1>🎉 Welcome to Tippr Dashboard!</h1>
      <p>You are securely logged in.</p>
    </div>
  );
};

function App() {
  return (
    <Routes>
      {/* Publika rutter */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />

      {/* Skyddade rutter - Allt innanför ProtectedRoute kräver inloggning */}
      <Route element={<ProtectedRoute />}>
        <Route path="/" element={<Dashboard />} />
        {/* Här kommer vi lägga till /groups, /predictions etc sen */}
      </Route>

      {/* Fallback - Om ingen sida matchar, gå till hem */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default App;
