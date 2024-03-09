<template>
  <div class="card">
    <h2>Client Info</h2>
    <h3 v-if="isLoading || !client">Loading...</h3>
    <h3 v-else>{{ client.name }} {{ client.surname }}</h3>
    <h1>Balance: <span v-if="isLoading || !client">Loading...</span> <span v-else>{{ numberToCurrency(client.clientBalance) }}</span></h1>
  </div>
</template>

<script setup lang="ts">
import { numberToCurrency } from '@/helpers/helpers';
import type { ClientDTO } from '@/models/ClientDTO';
import type { PropType } from 'vue';

defineProps({
  client: {
    required: true,
    type: Object as PropType<ClientDTO | undefined>
  },
  isLoading: {
    required: true,
    type: Boolean
  }
  
  })
</script>

<style lang="scss" scoped>
  .card{
    backdrop-filter: blur(1px);
    border-radius: 1rem;
    border: 1px solid rgb(48, 48, 48);
    box-shadow: 0.05em 0.05em 1em rgba(255, 255, 255, 0.1);
    padding: 1rem;

    h2{
      margin: 0;
      padding: 0;
    }

    h1,h3{
      margin-top: 0;
      margin-left: 1rem;
      padding: 0;
    }
  }
</style>