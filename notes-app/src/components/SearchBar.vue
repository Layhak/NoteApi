<template>
  <div class="relative">
    <input
      type="text"
      :value="modelValue"
      @input="updateValue"
      @keyup.enter="$emit('search', modelValue)"
      :placeholder="placeholder"
      class="w-full px-3 py-2 pr-10 border rounded-md"
    />
    <button
      @click="$emit('search', modelValue)"
      class="absolute right-2 top-1/2 transform -translate-y-1/2 text-gray-500"
    >
      🔍
    </button>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  modelValue: string
  placeholder?: string
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void
  (e: 'search', value: string): void
}>()

function updateValue(event: Event) {
  const target = event.target as HTMLInputElement
  emit('update:modelValue', target.value)
}
</script>
