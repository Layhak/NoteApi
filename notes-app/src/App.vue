<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useNotesStore } from '@/stores/notes'
import { onMounted, ref, watch } from 'vue'

// Initialize notes store
const notesStore = useNotesStore()

// Create a reactive reference for authentication state
const isAuthenticated = ref(!!localStorage.getItem('token'))

// Function to handle logout
const handleLogout = () => {
  localStorage.removeItem('token')
  isAuthenticated.value = false
  window.location.href = '/'
}

// Fetch notes when app loads (if user is authenticated)
onMounted(() => {
  if (isAuthenticated.value) {
    notesStore.fetchNotes()
  }
})
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Navigation Header -->
    <header class="bg-white shadow">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex items-center">
            <h1 class="text-xl font-bold text-blue-600">Notes App</h1>
          </div>

          <nav class="flex items-center space-x-4">
            <RouterLink
              to="/"
              class="px-3 py-2 rounded-md text-sm font-medium hover:bg-gray-100"
              :class="$route.path === '/' ? 'text-blue-600' : 'text-gray-700'"
            >
              Home
            </RouterLink>

            <RouterLink
              to="/notes"
              class="px-3 py-2 rounded-md text-sm font-medium hover:bg-gray-100"
              :class="$route.path.includes('/notes') ? 'text-blue-600' : 'text-gray-700'"
            >
              My Notes
            </RouterLink>

            <template v-if="!isAuthenticated">
              <RouterLink
                to="/login"
                class="px-3 py-2 rounded-md text-sm font-medium hover:bg-gray-100"
                :class="$route.path === '/login' ? 'text-blue-600' : 'text-gray-700'"
              >
                Login
              </RouterLink>

              <RouterLink
                to="/register"
                class="px-3 py-2 rounded-md text-sm font-medium hover:bg-gray-100"
                :class="$route.path === '/register' ? 'text-blue-600' : 'text-gray-700'"
              >
                Register
              </RouterLink>
            </template>

            <button
              v-else
              @click="handleLogout"
              class="px-3 py-2 rounded-md text-sm font-medium text-gray-700 hover:bg-gray-100"
            >
              Logout
            </button>
          </nav>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
      <RouterView />
    </main>

    <!-- Footer -->
    <footer class="bg-white border-t border-gray-200 py-4">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <p class="text-center text-sm text-gray-500">
          Notes App &copy; {{ new Date().getFullYear() }}
        </p>
      </div>
    </footer>
  </div>
</template>
