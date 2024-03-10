<template>
  <div class="client card">
    <div class="client-row">
      <div class="client-row-item">
        <p>{{ client.name }}</p>
      </div>
      <div class="client-row-item">
        <p>{{ client.surname }}</p>
      </div>
      <div class="client-row-item">
        <p
          class="money"
          :class="client.clientBalance > 0 ? 'positive' : 'negative'"
        >
          {{ numberToCurrency(client.clientBalance) }}
        </p>
      </div>
      <div class="client-row-end">
        <RouterLink
          role="button"
          :to="`/client/${props.client.clientID}`"
        >
          <button class="btn pill">View</button>
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { numberToCurrency } from '@/helpers/helpers'
  import type { ClientDTO } from '@/models/ClientDTO'
  import type { PropType } from 'vue'

  const props = defineProps({
    client: {
      required: true,
      type: Object as PropType<ClientDTO>
    }
  })
</script>

<style lang="scss" scoped>
  .client {
    display: flex;
    flex-direction: column;
    margin-bottom: 0.5rem;

    .client-row {
      display: flex;
      flex-direction: row;
      padding: 0.0125rem 0;

      .client-row-item {
        display: flex;
        align-items: center;

        width: 30%;

        p {
          width: 100%;
          margin: 0;
          padding: 0;
          text-align: center;
        }

        p.money {
          text-align: right;
          margin-right: 1rem;
        }
      }

      .client-row-end {
        width: 10%;

        button.btn {
          width: 100%;
        }
      }
    }
  }
</style>
