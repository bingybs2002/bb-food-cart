import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem
} from "@/components/ui/sidebar"
import {Link} from "react-router-dom"
import { User2 } from "lucide-react"
import { Separator } from "./ui/separator"

export function AppSidebar() {
  return (
    <Sidebar>
      <SidebarHeader>
        <Link to="/">
          <div className="flex justify-center">
            <img src="/bbFoodCart.png"  className=" w-30 h-auto " />
              <a href="#" className="flex items-center text-2xl">
              BB Food Cart
              </a>
          </div>
        </Link>
        <Separator/>
      </SidebarHeader>

{/*Content*/}
      <SidebarContent className="py-4">
        <SidebarGroup className="space-y-5">
          <div>
            <SidebarGroupLabel className="ml-2 text-medium font-bold">
                Orders
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
              <SidebarMenu className="space-y-1">
              <SidebarMenuItem><Link to="/Orders/OrderHistory">Order History</Link></SidebarMenuItem>
              <SidebarMenuItem><Link to="/Orders/ShoppingCartTracker">Shopping Cart Tracker</Link></SidebarMenuItem>
              </SidebarMenu>
            </SidebarGroupContent>
          </div>

          <div>
            <SidebarGroupLabel className="ml-2 text-medium font-bold">
                Menu Management
              </SidebarGroupLabel>
              <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                  <SidebarMenuItem><Link to="/Menu/ViewMenu">View Menu</Link></SidebarMenuItem>
                  <SidebarMenuItem><Link to="/Menu/AddItem">Add New Item</Link></SidebarMenuItem>
                  <SidebarMenuItem><Link to="/Menu/EditItem">Edit Existing Item</Link></SidebarMenuItem>
                  <SidebarMenuItem><Link to="/Menu/DeleteItem">Delete Item</Link></SidebarMenuItem>
                </SidebarMenu>
              </SidebarGroupContent>
            </div>


            <div>
            <SidebarGroupLabel className="ml-2 text-medium font-bold">
                Gacha
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                <SidebarMenuItem><Link to="/Gacha/PricePool">Prize Pool</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="/Gacha/Rules">Rules</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="/Gacha/History">History</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="/Gacha/addItem">Add items</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Delete item</Link></SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
            </div>

           <div>
            <SidebarGroupLabel className="ml-2 text-medium font-bold">
                User Information
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                <SidebarMenuItem><Link to="">User Admin Status</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">User Profile</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">User Order history</Link></SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
            </div>

           <div>
            <SidebarGroupLabel className="ml-2 text-medium font-bold">
                Utilities
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                <SidebarMenuItem><Link to="/Utilities/AllergicEnumConverter">Allergic enum converter</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">User Profile</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">User Order history</Link></SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
            </div>

        </SidebarGroup>
        </SidebarContent>

{/*Footer*/}
      <SidebarFooter>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton>
              <User2 />
              Administrator
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  )
}