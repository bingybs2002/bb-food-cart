import { Button } from "@/components/ui/button"
import {Route, Routes, Link} from "react-router-dom"
import LoginPage from "./Pages/LoginPage"
import SignupPage from "./Pages/SignupPage"
import AdminPanel from "./Pages/AdminPanel"
import GachaRulesPage from "./Pages/Gacha/GachRulesPage"

function MainPage() {
  const accessToken = localStorage.getItem("accessToken")
  const expiredTime = localStorage.getItem("accessTokenExpiresAtUtc")
  const isExpired = !expiredTime || new Date(expiredTime) < new Date()
  if (accessToken && !isExpired) {
    return <AdminPanel />

  }
  return (
        <div className="flex min-h-svh p-6">
          <div className="flex max-w-md min-w-0 flex-col gap-4 text-sm leading-loose">
            <div>
              <h1 className="font-medium">BB-FOOD is in developement!</h1>
              <p>Would add features later as they are being developed. If you are in this page,
                it would've mean that you are either not logged in or the session has expired.
                Please login again, or create an account. 
              </p>
              <Button>
                <Link to="/login">Login</Link>
              </Button>
            </div>
            <div className="font-mono text-xs text-muted-foreground">
            </div>
          </div>
        </div>
  )
}

export default function App(){
  return(
    <Routes>
      <Route path="/" element={<MainPage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignupPage />} />
      <Route path="/homepage" element={<AdminPanel />} />
      <Route path="/Gacha/Rules" element={<GachaRulesPage />} />
    </Routes>
  )
}
