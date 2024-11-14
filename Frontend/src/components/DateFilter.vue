<template>
  <div class="q-pa-xl">
    <text class="text-dark-100 text-15px font-medium">{{title}}</text>
    <q-input class="w-max" v-model="updatedDate">
      <template v-slot:append>
        <q-icon name="event" class="cursor-pointer">
          <q-popup-proxy cover transition-show="scale" transition-hide="scale">
            <q-date v-model="updatedDate" mask="HH:mm DD MMMM YYYY">
              <div class="row items-center justify-end">
                <q-btn v-close-popup label="Close" color="primary" flat />
              </div>
            </q-date>
          </q-popup-proxy>
        </q-icon>
      </template>
      <template v-slot:prepend>
        <q-icon name="access_time" class="cursor-pointer">
          <q-popup-proxy cover transition-show="scale" transition-hide="scale">
            <q-time v-model="updatedDate" mask="HH:mm DD MMMM YYYY" format24h>
              <div class="row items-center justify-end">
                <q-btn v-close-popup label="Close" color="primary" flat />
              </div>
            </q-time>
          </q-popup-proxy>
        </q-icon>
      </template>
    </q-input>
  </div>
</template>

<script lang="ts" setup>
import { QInput, QIcon, QPopupProxy, QDate, QBtn, QTime } from 'quasar';
import { computed } from 'vue';

const props = defineProps<{
  dateFilter: string;
  title:string;
}>();

const updatedDate = computed<string>({
  get() {
    return props.dateFilter;
  },
  set(val) {
    return emit('update', val);
  },
});
const emit = defineEmits(['update']);
</script>
