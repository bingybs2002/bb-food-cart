import { Button } from "@/components/ui/button"
import {Route, Routes, Link} from "react-router-dom"
import LoginPage from "./Pages/Auth/LoginPage"
import SignupPage from "./Pages/Auth/SignupPage"
import AdminPanel from "./Pages/AdminPanel"
import GachaRulesPage from "./Pages/Gacha/GachRulesPage"
import Utilities from "./Pages/Utilities"
import OrderHistory from "./Pages/Orders/OrderHistory/OrderHistory"
import ShoppingCartTracker  from "./Pages/Orders/ShoppingCartTracker/ShoppingCartTracker"
import AddItem from "./Pages/Menu/AddItem"
import ViewMenu from "./Pages/Menu/ViewMenu"
import DeleteItem from "./Pages/Menu/DeleteItem"
import EditItem from "./Pages/Menu/EditItem"
import GachaManagement from "./Pages/Gacha/gachamanagement"
import GachaPricePool from "./Pages/Gacha/PricePool/PricePool"

function MainPage() {
  const accessToken = localStorage.getItem("accessToken")
  const expiredTime = localStorage.getItem("accessTokenExpiresAtUtc")
  const isExpired = !expiredTime || new Date(expiredTime) < new Date()
  if (accessToken && !isExpired) {
    return <AdminPanel />
  }
  if(accessToken&&isExpired){
    localStorage.clear()
  }
  return (
        <div className="flex min-h-svh p-6">
          <div className="flex max-w-md min-w-0 flex-col gap-4 text-sm leading-loose">
            <div>
              <h1 className="font-medium">BB-FOOD is in development!</h1>
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
      //Landing Page
      <Route path="/" element={<MainPage/>} />
      //Auth
      <Route path="/login" element={<LoginPage/>} />
      <Route path="/signup" element={<SignupPage/>} />
      //homepage
      <Route path="/homepage" element={<AdminPanel/>} />
      //gacha
      <Route path="/Gacha/Rules" element={<GachaRulesPage/>} />
      <Route path="/Gacha/management" element={<GachaManagement/>}/>
      <Route path="/Gacha/pricePool" element={<GachaPricePool/>}/>
      //Utils
      <Route path="/Utilities" element={<Utilities/>}/>
      //order
      <Route path="/Orders/OrderHistory" element={<OrderHistory/>}/>
      <Route path="/Orders/ShoppingCartTracker" element={<ShoppingCartTracker/>}/>
      //menu
      <Route path="/Menu/AddItem" element={<AddItem/>}/>
      <Route path="/Menu/EditItem" element={<EditItem/>}/>
      <Route path="/Menu/DeleteItem" element={<DeleteItem/>}/>
      <Route path="/Menu/ViewMenu" element={<ViewMenu/>}/>
    </Routes>
  )
}
