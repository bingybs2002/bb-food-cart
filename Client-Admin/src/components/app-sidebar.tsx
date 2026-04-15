import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupAction,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarTrigger,
} from "@/components/ui/sidebar"
import {Link} from "react-router-dom"
import {
    DropdownMenu, 
    DropdownMenuTrigger, 
    DropdownMenuContent,
    DropdownMenuItem,
} from "@/components/ui/dropdown-menu"
import { ChevronDown, Divide, Plus, User2 } from "lucide-react"
import { Separator } from "./ui/separator"

export function AppSidebar() {
  return (
    <Sidebar>
{/*Header*/}
        <SidebarHeader>
            <div className="flex justify-center">
                <img src="/bbFoodCart.png" alt="bb food cart.png" className=" w-30 h-auto " />
                <a href="#" className="flex items-center text-2xl">
                BB Food Cart
                </a>
            </div>
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
                <SidebarMenuItem><Link to="">Order History</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Shopping Cart Tracker</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Order History</Link></SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
            </div>

            <div>
            <SidebarGroupLabel className="ml-2 text-medium font-bold">
                Menu Management
              </SidebarGroupLabel>
              <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                  <SidebarMenuItem><Link to="">View Menu</Link></SidebarMenuItem>
                  <SidebarMenuItem><Link to="">Add New Item</Link></SidebarMenuItem>
                  <SidebarMenuItem><Link to="">Edit Existing Item</Link></SidebarMenuItem>
                  <SidebarMenuItem><Link to="">Delete Item</Link></SidebarMenuItem>
                </SidebarMenu>
              </SidebarGroupContent>
            </div>


            <div>
            <SidebarGroupLabel className="ml-2 text-medium font-bold">
                Gacha
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                <SidebarMenuItem><Link to="">Mock Testing</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Prize Pool</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Rules</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">History</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Add items</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Delete item</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Delete All Items</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Order History</Link></SidebarMenuItem>
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
              Adminstrator
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  )
}