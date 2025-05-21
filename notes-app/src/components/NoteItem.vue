<template>
  <div class="border rounded-lg overflow-hidden shadow-sm hover:shadow-md transition-shadow">
    <div class="p-4">
      <h3 class="font-bold text-lg mb-1 truncate">{{ note.title }}</h3>
      <p class="text-sm text-gray-500 mb-2">
        {{ formatDate(note.updatedAt) }}
      </p>
      <p class="text-gray-700 line-clamp-3 mb-4">
        {{ note.content || 'No content' }}
      </p>
      <div class="flex justify-end gap-2">
        <button @click="$emit('view', note)" class="text-blue-500 hover:text-blue-700">View</button>
        <button @click="$emit('edit', note)" class="text-green-500 hover:text-green-700">
          Edit
        </button>
        <button @click="$emit('delete', note)" class="text-red-500 hover:text-red-700">
          Delete
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Note } from '@/types'

defineProps<{
  note: Note
}>()

defineEmits<{
  (e: 'view', note: Note): void
  (e: 'edit', note: Note): void
  (e: 'delete', note: Note): void
}>()

function formatDate(dateString: string): string {
  const date = new Date(dateString)
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  }).format(date)
}
</script>
