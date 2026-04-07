import { Button } from "@/components/ui/button"
import {Route, Routes, Link} from "react-router-dom"
import LoginPage from "./Pages/LoginPage"
import SignupPage from "./Pages/SignupPage"

function Homepage() {
  return (
        <div className="flex min-h-svh p-6">
          <div className="flex max-w-md min-w-0 flex-col gap-4 text-sm leading-loose">
            <div>
              <h1 className="font-medium">BB-FOOD is in developement!</h1>
              <p>Would add features later as they are being developed.</p>
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
      <Route path="/" element={<Homepage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignupPage />} />
    </Routes>
  )
}
