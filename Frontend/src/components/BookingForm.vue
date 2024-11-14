<template>
  <q-form class="ml-5 mr-5 text-center">
    <div class="mt-5 mb-2">
      <text class="text-dark font-semibold text-20px"
        >{{ props.bookingState }} Booking</text
      >
    </div>
    <q-separator rounded color="dark" class="mr-2 ml-2" />
    <q-input
      class="mt-3"
      clearable
      clear-icon="clear"
      type="text"
      v-model="props.booking!.objectId"
      label="Object Id"
    />
    <q-input
      class="mt-3"
      clearable
      clear-icon="clear"
      type="text"
      v-model="props.booking!.name"
      label="Name"
    />
    <q-input
      class="mt-3"
      clearable
      clear-icon="clear"
      type="text"
      v-model="props.booking!.email"
      label="Email"
    />
    <q-input
      class="mt-3"
      clearable
      clear-icon="clear"
      type="text"
      v-model="props.booking!.phoneNumber"
      label="Phone Number"
    />
    <q-input class="mt-3" v-model="updatedDate">
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
    <q-btn
      v-if="props.bookingState == 'Edit'"
      class="mt-5"
      label="Submit"
      @click.prevent="onSubmit"
      type="submit"
      color="primary"
    />
    <q-btn
      v-else
      class="mt-5"
      label="Add"
      @click.prevent="onSubmit"
      type="add"
      color="positive"
    />
    <q-btn
      @click.prevent="onCancel"
      label="Cancel"
      color="negative"
      class="mt-5 ml-5"
      >{{
    }}</q-btn>
    <q-btn
      v-if="props.bookingState == 'Edit'"
      @click.prevent="onDelete"
      icon="directions"
      label="Delete"
      color="negative"
      class="mt-5 ml-5"
    />
  </q-form>
  {{props.booking}}
</template>

<script lang="ts" setup>
import { Booking } from '@/model/booking';
import { useBookingsStore } from '@/stores/bookingStore';
import {
  QForm,
  QSeparator,
  QInput,
  QIcon,
  QPopupProxy,
  QDate,
  QBtn,
  QTime,
} from 'quasar';
import { computed} from 'vue';
import { v4 as uuidv4 } from 'uuid';

const { addBooking, deleteBooking, updateBooking } = useBookingsStore();

const props = defineProps<{
  booking: Booking ,
  bookingState: string;
  selDate: string;
  facilityId: string,
}>();
const updatedDate = computed<string>({
  get() {
    return props.selDate;
  },
  set(val) {
    return emit('update', val);
  },
});

const emit = defineEmits([
  'changeState',
  'changeDrawerRight',
  'update',
  'updateTable',
]);
const onSubmit = async () => {
  props.booking.facilityId = props.facilityId;
  const dParse = Date.parse(updatedDate.value);
  props.booking.bookingDate = new Date(dParse);
  console.log(props.facilityId)
  if (props.bookingState == 'Edit') {
    await updateBooking(props.booking!);
  } else {
    props.booking.id = uuidv4();
    await addBooking(props.booking!);
  }
  if (props.bookingState == 'Add') {
    emit('changeState');
  }
  await emit('updateTable');
};
const onDelete = async () => {
  await deleteBooking(props.booking!);
  await onCancel();
};
const onCancel = async () => {
  emit('changeDrawerRight');
  updatedDate.value = '';
  props.booking.id = '';
  props.booking.bookingDate = new Date();
  props.booking.email = '';
  props.booking.facilityId = '';
  props.booking.name = '';
  props.booking.objectId = '';
  props.booking.phoneNumber = '';
  await emit('updateTable');
};
</script>

<style lang="scss" scoped></style>
