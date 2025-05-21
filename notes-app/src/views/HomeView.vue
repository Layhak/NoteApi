<template>
  <div class="py-12">
    <div class="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="text-center">
        <h1 class="text-4xl font-extrabold text-gray-900 sm:text-5xl sm:tracking-tight lg:text-6xl">
          Welcome to Notes App
        </h1>
        <p class="mt-5 max-w-xl mx-auto text-xl text-gray-500">
          A simple way to organize your thoughts, ideas, and tasks.
        </p>
        <div class="mt-8 flex justify-center">
          <div class="inline-flex rounded-md shadow">
            <RouterLink
              to="/notes"
              class="inline-flex items-center justify-center px-5 py-3 border border-transparent text-base font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700"
            >
              Get Started
            </RouterLink>
          </div>
          <div class="ml-3 inline-flex">
            <RouterLink
              to="/login"
              class="inline-flex items-center justify-center px-5 py-3 border border-transparent text-base font-medium rounded-md text-blue-700 bg-blue-100 hover:bg-blue-200"
            >
              Login
            </RouterLink>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
