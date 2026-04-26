import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"

export default function GachaRulesPage() {
  return (
    <SidebarProvider>
      <AppSidebar />

      <main className="ml-10 flex-1 p-6 max-w-2xl border-1">
        <h1 className="text-2xl font-bold text-center">Gacha Rules and Disclaimers:</h1>

        <p className ="mt-4">
          Compared to traditional Chinese food Experience, we added Gacha on top of Fortune Cookies!
        </p>

        <div>
          <h3 className="text-lg font-semibold mb-2 mt-6">
            Drop Rate on Count of 30
          </h3>

          <table className="border-3 border-gray-300 text-sm">
              <tr>
                <th className="border px-3 py-2">Rarity</th>
                <th className="border px-3 py-2">Expected Rate</th>
                <th className="border px-3 py-2">Count (of 30)</th>
                <th className="border px-3 py-2">Actual Rate (%)</th>
              </tr>
            <tbody>
              <tr>
                <td className="border px-3 py-2">Common</td>
                <td className="border px-3 py-2">50%</td>
                <td className="border px-3 py-2">15</td>
                <td className="border px-3 py-2">50%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Rare</td>
                <td className="border px-3 py-2">30%</td>
                <td className="border px-3 py-2">9</td>
                <td className="border px-3 py-2">30%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Epic</td>
                <td className="border px-3 py-2">12%</td>
                <td className="border px-3 py-2">3</td>
                <td className="border px-3 py-2">10%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Mystic</td>
                <td className="border px-3 py-2">7%</td>
                <td className="border px-3 py-2">2</td>
                <td className="border px-3 py-2">6.66%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Legendary</td>
                <td className="border px-3 py-2">1%</td>
                <td className="border px-3 py-2">1</td>
                <td className="border px-3 py-2">3.33%</td>
              </tr>
            </tbody>
          </table>

        </div>
        <SidebarTrigger className="ml-10 mt-100"/>
      </main>
    </SidebarProvider>
  )
}